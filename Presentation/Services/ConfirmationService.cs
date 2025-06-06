using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

// Service for confirmation database handling, copilot assisted.
namespace Presentation.Services {
    public class ConfirmationService {
        private readonly DataContext _context;

        public ConfirmationService(DataContext context) {
            _context = context;
        }

        // Get all confirmations
        public async Task<IEnumerable<ConfirmationEntity>> GetAllAsync() {
            return await _context.Confirmations.ToListAsync();
        }

        // Get a single confirmation by eventId
        public async Task<ConfirmationEntity?> GetAsync(string eventId) {
            return await _context.Confirmations.FirstOrDefaultAsync(x => x.Id == eventId);
        }

        // Add confirmations to the database
        public async Task<bool> AddConfirmationsAsync(List<ConfirmationEntity> confirmations) {
            _context.Confirmations.AddRange(confirmations);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}