namespace RunBase_API.Models
{
    using Microsoft.EntityFrameworkCore;
    using RunBase_API.Models;

    public class RajtszamGenerator
    {
        private readonly RunBaseDbContext _context;

        public RajtszamGenerator(RunBaseDbContext context)
        {
            _context = context;
        }

        public async Task<int> KovetkezoRajtszamAsync(int versenyId, int tav)
        {
            var legnagyobbRajtszam = await _context.Versenyindulas
                .Where(v => v.VersenyId == versenyId && v.Tav == tav)
                .MaxAsync(v => (int?)v.Rajtszam);

            return (legnagyobbRajtszam ?? 0) + 1;
        }
    }

}
