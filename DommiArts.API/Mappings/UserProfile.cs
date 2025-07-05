using AutoMapper;
using DommiArts.API.Models;
using DommiArts.API.DTOs.User;
public class UserProfile : Profile
{
    public UserProfile()
    {
        // Registro de usuário (RegisterUserDTO → User)
        CreateMap<UserCreateDTO, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // gerado pelo banco
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // definido manualmente
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore()) // definido manualmente
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // gerenciado pelo sistema
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // gerenciado pelo sistema
            .ForMember(dest => dest.IsActive, opt => opt.Ignore()) // estado gerenciado pela aplicação
            .ForMember(dest => dest.Role, opt => opt.Ignore()); // definido manualmente ou em lógica de negócio

        // Atualização de usuário (UserUpdateDTO → User)
        CreateMap<UserUpdateDTO, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id não deve ser atualizado
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // alteração de senha seria um fluxo separado
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // não altera criação
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow)) // atualiza UpdatedAt
            .ForMember(dest => dest.IsActive, opt => opt.Ignore()) // atualizado em fluxo separado (ativação/desativação)
            .ForMember(dest => dest.Role, opt => opt.Ignore()); // papel do usuário controlado por admins
    }
}
