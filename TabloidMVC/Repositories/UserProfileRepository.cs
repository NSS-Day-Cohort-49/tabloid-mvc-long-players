﻿using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              uT.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType uT ON u.UserTypeId = uT.id
                         ORDER BY LastName
                       ";
                    
                    var reader = cmd.ExecuteReader();

                    var categories = new List<UserProfile>();

                    while (reader.Read())
                    {
                        categories.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        });
                    }

                    reader.Close();

                    return categories;
                }
            }
        }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              uT.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType uT ON u.UserTypeId = uT.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetUserProfileById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              uT.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType uT ON u.UserTypeId = uT.id
                       WHERE u.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        UserProfile userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };

                        reader.Close();
                        return userProfile;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }

                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                            SET 
                                Email = @Email, 
                                FirstName = @firstName, 
                                LastName = @lastName, 
                                DisplayName = @displayName,
                                ImageLocation = @imageLocation,
                                UserTypeId = @userTypeId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@firstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@imageLocation", userProfile.ImageLocation);
                    
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStatus(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE UserProfile
                    DisplayName, FirstName, LastName, Email, ImageLocation, UserTypeId
                    VALUES (@DisplayName, @FirstName, @LastName, @Email, @ImageLocation, 3)
                    WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@firstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@imageLocation", userProfile.ImageLocation);
                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);


                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM UserProfile
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", userProfileId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
