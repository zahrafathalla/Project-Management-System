using AutoMapper;

namespace ProjectManagementSystem.Helper
{
    public static class MapperHandler
    {
        public static IMapper mapper {  get; set; }
        public static TResult Map<TResult>(this object source)
        {

            return mapper.Map<TResult>(source);
        }
    }
}
