using FluentValidation;
using GerenciadorTarefas.Application.Authentication.Inteface;
using GerenciadorTarefas.Application.Mapper;
using GerenciadorTarefas.Application.Service.Interface;
using GerenciadorTarefas.Application.Validation;
using GerenciadorTarefas.Communication.Request;
using GerenciadorTarefas.Domain.Model;
using GerenciadorTarefas.Domain.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Application.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IValidator<RequestUsuario> _validatorUser;
        private readonly MappingTo _mapper;

        public UsuarioService(IUsuarioRepository repository, IJwtTokenGenerator tokenGenerator, MappingTo mapper, IValidator<RequestUsuario> validatorUser)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
            _validatorUser = validatorUser;
            _mapper = mapper; 
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<Usuario> CreateUserAsync(RequestUsuario request)
        {
            var resultValidator = await _validatorUser.ValidateAsync(request);

            if (!resultValidator.IsValid) 
            {
                throw new ValidationException(resultValidator.Errors);
            }

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var usuario = _mapper.MapToUsuario(request);

            usuario.Password = senhaHash;

            await _repository.CreateAsync(usuario);

            return usuario;
        }

        public async Task<bool> UpdateUserAsync(RequestUsuario request)
        {
            var usuario = await _repository.GetByIdAsync(request.Id);

            if (usuario == null)
                return false;

            if (!string.IsNullOrEmpty(request.Name)) usuario.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Email)) usuario.Email = request.Email;
            if (!string.IsNullOrEmpty(request.Password)) usuario.Password = request.Password;
            if (request.DataCriacao.HasValue) usuario.DataCriacao = request.DataCriacao.Value;
            if (request.IsActive.HasValue) usuario.IsActive = request.IsActive.Value;

            await _repository.UpdateAsync(usuario);

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<string> AuthenticateAsync(string email, string senha)
        {
            var usuario = await _repository.GetByEmailAsync(email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.Password))
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            return _tokenGenerator.GenerateToken(usuario.Id, usuario.Name, usuario.Email);
        }

        public async Task<bool> GetByEmailAsync(string email)
        {
            var emailEncontrado = await _repository.GetByEmailAsync(email);
            if (emailEncontrado != null) 
            { 
                return true; 
            }
            return false;
        }
    }

}
