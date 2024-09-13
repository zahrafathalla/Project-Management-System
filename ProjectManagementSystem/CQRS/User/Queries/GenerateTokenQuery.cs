using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Authontication;
using ProjectManagementSystem.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagementSystem.CQRS.User.Queries;

public class GenerateTokenQuery : IRequest<(string Token, int ExpiresIn)>
{
    public Data.Entities.User User { get; set; }
}

public class GenerateTokenQueryHandler : IRequestHandler<GenerateTokenQuery, (string Token, int ExpiresIn)>
{
    private readonly JwtOptions _options;

    public GenerateTokenQueryHandler(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    
    public Task<(string Token, int ExpiresIn)> Handle(GenerateTokenQuery request, CancellationToken cancellationToken)
    {
        Claim[] claims = new Claim[]
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

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        var expiresIn = _options.ExpiryMinutes * 60;

        return Task.FromResult((jwtToken, expiresIn));
    }
}
