using Ensco.Data;
using Ensco.Utilities;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Scheduler
{
    public static class SchedulerService
    {
        private static bool processingAD = false;
        public static void ProcessActiveDirectoryUsersJob(string adName)
        {
            string AD = adName;

            if (processingAD)
                return;

            try
            {
                processingAD = true;
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + AD + "/dc=ensco,dc=ws");
                DirectorySearcher searcher1 = new DirectorySearcher(entry);
                EnscoMasterRepository<Ensco.Data.Master_Users> entities = new EnscoMasterRepository<Ensco.Data.Master_Users>();

                List<Ensco.Data.Master_Users> userEntities = entities.GetAll().Cast<Ensco.Data.Master_Users>().ToList();

                foreach (Ensco.Data.Master_Users user in userEntities)
                {
                    // REMOVE THIS CODE
                    if (user.DisplayName != null && user.DisplayName.Length > 0)
                        continue;

                    string passport = user.Passport.Trim();
                    searcher1.Filter = "(&(objectClass=user)(sAMAccountName=" + passport + "))";
                    SearchResult r = searcher1.FindOne();
                    if (r != null)
                    {
                        DirectoryEntry de = r.GetDirectoryEntry();
                        user.DisplayName = de.Properties["DisplayName"][0].ToString();
                        //string email = de.Properties["mail"][0].ToString();
                        //user.Email = email;
                        entities.Edit(user);
                        entities.Save();
                    }
                }

                // Update managers after updating display name, AD only has manager name
                foreach (Ensco.Data.Master_Users user in userEntities)
                {
                    // REMOVE THIS CODE
                    if (user.Manager != null && user.Manager > 0)
                        continue;

                    string passport = user.Passport.Trim();

                    try
                    {
                        searcher1.Filter = "(&(objectClass=user)(sAMAccountName=" + passport + "))";
                        SearchResult r = searcher1.FindOne();
                        if (r != null)
                        {
                            DirectoryEntry de = r.GetDirectoryEntry();
                            string manager = de.Properties["manager"][0].ToString();
                            char[] sep = { ',' };
                            char[] sep2 = { '=' };
                            string[] tokens = manager.Split(sep);
                            string token = tokens.FirstOrDefault(x => x.Contains("CN"));
                            string[] tokens2 = token.Split(sep2);
                            manager = (tokens2.Length > 1) ? tokens2[1] : null;
                            manager = manager.Trim();
                            if (manager != null)
                            {
                                char[] sep3 = { ' ' };
                                string[] names = manager.Split(sep3);

                                Ensco.Data.Master_Users managerUser = null;
                                if (names.Length == 3)
                                {
                                    managerUser = userEntities.FirstOrDefault(x => x.FirstName == names[0] && x.MiddleName == names[1] && x.LastName == names[2]);
                                }
                                else if (names.Length == 2)
                                {
                                    managerUser = userEntities.FirstOrDefault(x => x.FirstName == names[0] && x.LastName == names[1]);
                                }
                                else if (names.Length == 4)
                                {
                                    string lastname = names[2] + " " + names[3];
                                    managerUser = userEntities.FirstOrDefault(x => x.FirstName == names[0] && x.MiddleName == names[1] && x.LastName == lastname);
                                }
                                if (managerUser != null)
                                {
                                    user.Manager = managerUser.Id;
                                    entities.Edit(user);
                                    entities.Save();
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        int n = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(new LogInfo(MethodBase.GetCurrentMethod(), ex.Message));
            }
            processingAD = false;
        }
    }
}
