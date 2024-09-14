using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Authontication;
using ProjectManagementSystem.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementSystem.CQRS.Users.Commands;

public class GenerateTokenCommand : IRequest<string>
{
    public User? User { get; set; }
}

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, string>
{
    private readonly JwtOptions _options;

    public GenerateTokenCommandHandler(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }


    public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        Claim[] claims =
        {
            new(JwtRegisteredClaimNames.Email, request.User.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };


        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: singingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
