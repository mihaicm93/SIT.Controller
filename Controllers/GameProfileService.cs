using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace SIT.Controller.Controllers
{
    public class GameProfileService
    {
        private readonly string profilesDirectory;

        public GameProfileService(IConfiguration configuration)
        {
            var serverPath = configuration["ServerPath"];
            profilesDirectory = Path.Combine(serverPath, "user", "profiles");
        }

        public class UserProfile
        {
            public Info info { get; set; }

            public class Info
            {
                public string id { get; set; }
                public string scavId { get; set; }
                public int aid { get; set; }
                public string username { get; set; }
                public string password { get; set; }
                public bool wipe { get; set; }
                public string edition { get; set; }
            }
        }

        public void SaveProfile(UserProfile updatedProfile, string filePath)
        {
            try
            {
                // Serialize the updated profile
                var updatedJsonString = JsonSerializer.Serialize(updatedProfile, new JsonSerializerOptions { WriteIndented = true });

                // Save the updated profile to the file
                File.WriteAllText(filePath, updatedJsonString);

                // Log the successful save operation
                Console.WriteLine($"Profile saved successfully: {filePath}");
            }
            catch (Exception ex)
            {
                // Log any errors that occur during the save operation
                Console.WriteLine($"Error saving profile to file: {ex.Message}");
            }
        }

        public List<UserProfile> SearchProfilesByUsername(string username)
        {
            var profiles = new List<UserProfile>();

            foreach (var file in Directory.GetFiles(profilesDirectory, "*.json"))
            {
                try
                {
                    // Use JsonDocument for streaming JSON parsing
                    using (var fileStream = File.OpenRead(file))
                    {
                        var jsonDocument = JsonDocument.Parse(fileStream);
                        var root = jsonDocument.RootElement;

                        // Deserialize the UserProfile from the JSON document
                        var userProfile = new UserProfile
                        {
                            info = new UserProfile.Info
                            {
                                id = root.GetProperty("info").GetProperty("id").GetString(),
                                scavId = root.GetProperty("info").GetProperty("scavId").GetString(),
                                aid = root.GetProperty("info").GetProperty("aid").GetInt32(),
                                username = root.GetProperty("info").GetProperty("username").GetString(),
                                password = root.GetProperty("info").GetProperty("password").GetString(),
                                wipe = root.GetProperty("info").GetProperty("wipe").GetBoolean(),
                                edition = root.GetProperty("info").GetProperty("edition").GetString()
                            }
                        };

                        // Add the deserialized profile to the list
                        if (userProfile.info.username.Equals(username, StringComparison.OrdinalIgnoreCase))
                        {
                            profiles.Add(userProfile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            return profiles;
        }
    }
}
