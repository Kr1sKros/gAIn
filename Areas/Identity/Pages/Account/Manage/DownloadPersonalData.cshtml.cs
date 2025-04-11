// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using gain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using gain.Data;
using Microsoft.Extensions.Logging;
using NuGet.Packaging;

namespace gain.Areas.Identity.Pages.Account.Manage
{

    public class SessionWithEquipmentsForDownloadOnly
    {
        public int Id { get; set; }
    
        public DateTime SessionStartTime { get; set; }
    
        public int Duration { get; set; }

        public string FkUserId { get; set; } = string.Empty;
        
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
    
    
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            UserManager<ApplicationUser> userManager,
            ILogger<DownloadPersonalDataModel> logger,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, object>();
            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider}ExternalLoginProviderKey", l.ProviderKey);
            }
            personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

            personalData.Add("AchievementLevels:", user.AchievementLevels);
            
            var list = new List<SessionWithEquipmentsForDownloadOnly>();
            foreach (var ses in _db.TrainingSessions)
            {
                if (ses.FkUserId == user.Id)
                {
                    var a = new SessionWithEquipmentsForDownloadOnly();
                    a.Id = ses.Id;
                    a.SessionStartTime = ses.SessionStartTime;
                    a.Duration = ses.Duration;
                    a.FkUserId = ses.FkUserId;
                    foreach (var exId in ses.SessionExercises)
                    {
                        foreach (var dbExercise in _db.Exercises)
                        {
                            if (dbExercise.Id == exId)
                            {
                                a.Exercises.Add(dbExercise);
                            }
                        }
                    }
                    list.Add(a);
                }
            }
            personalData.Add("TrainingSessions", list);

            Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}
