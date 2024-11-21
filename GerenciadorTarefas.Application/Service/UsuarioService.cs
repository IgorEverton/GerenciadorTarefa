using GerenciadorTarefas.Application.Authentication.Inteface;
using GerenciadorTarefas.Application.Service.Interface;
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

        public UsuarioService(IUsuarioRepository repository, IJwtTokenGenerator tokenGenerator)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Usuario> CreateAsync(RequestUsuario request)
        {
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                Password = senhaHash
            };

            await _repository.CreateAsync(usuario);

            return usuario;
        }

        public async Task<bool> UpdateAsync(RequestUsuario request)
        {
            var usuario = await _repository.GetByIdAsync(request.Id);

            if (usuario == null) return false;

            usuario.Nome = request.Nome;
            usuario.Email = request.Email;
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            return await _repository.UpdateAsync(usuario);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<string> AuthenticateAsync(string email, string senha)
        {
            var usuario = await _repository.GetByEmailAsync(email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(senha, usuario.Password))
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            return _tokenGenerator.GenerateToken(usuario.Id, usuario.Nome, usuario.Email);
        }
    }

}
