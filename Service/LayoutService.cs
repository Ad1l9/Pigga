﻿using Microsoft.EntityFrameworkCore;
using Pigga.DAL;

namespace Pigga.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s=>s.Key, s=>s.Value);
            return settings;
        }
    }
}
