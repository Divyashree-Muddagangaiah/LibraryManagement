using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
namespace exampleplugin
{
    public class AccountPluginClass : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            Entity updateContact = new Entity(Constants.Contact);
            Entity updateAccount = new Entity();
            //if ((context.InputParameters.Contains("Target")) && (context.InputParameters["Target"] is Entity))
            if((context.PostEntityImages.Contains("PostImage"))&&(context.PostEntityImages["PostImage"] is Entity))
            {
               // updateAccount= (Entity)context.InputParameters["Target"];
                updateAccount = (Entity)context.PostEntityImages["PostImage"];
                if (updateAccount.LogicalName == Constants.Account)
                {
                    #region
                    if ((updateAccount.Attributes.Contains("new_userprovidingname")) && (updateAccount.Attributes["new_userprovidingname"] != null))
                    {
                        string accountName = (string)updateAccount.Attributes["new_userprovidingname"];
                        if((updateAccount.Attributes.Contains("primarycontactid")) &&(updateAccount.Attributes["primarycontactid"])!=null)
                        {
                            try
                            {
                                EntityReference contactId = (EntityReference)updateAccount.Attributes["primarycontactid"];
                                updateContact.Id = contactId.Id;
                                updateContact.Attributes["new_userprovidingname"] = accountName;
                                service.Update(updateContact);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidPluginExecutionException("invalid plugin "+ex.Message); 
                            }
                        }
                    }
                    #endregion

                    #region

                    if ((updateAccount.Attributes.Contains("new_country")) && (updateAccount.Attributes["new_country"] != null))
                    {
                      EntityReference Country = (EntityReference)updateAccount.Attributes["new_country"];
                         if((updateAccount.Attributes.Contains("primarycontactid")) &&(updateAccount.Attributes["primarycontactid"])!=null)
                        {
                            try
                            {
                                EntityReference contactId = (EntityReference)updateAccount.Attributes["primarycontactid"];
                                updateContact.Id = contactId.Id;
                                updateContact.Attributes["new_country"] = Country;
                                service.Update(updateContact);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidPluginExecutionException("invalid plugin "+ex.Message); 
                            }
                        }
                    }
                    #endregion

                    #region
                    if ((updateAccount.Attributes.Contains("new_type")) && (updateAccount.Attributes["new_type"] != null))
                    {
                        OptionSetValue type = (OptionSetValue)updateAccount.Attributes["new_type"];
                         if((updateAccount.Attributes.Contains("primarycontactid")) &&(updateAccount.Attributes["primarycontactid"])!=null)
                        {
                            try
                            {
                                EntityReference contactId = (EntityReference)updateAccount.Attributes["primarycontactid"];
                                updateContact.Id = contactId.Id;
                                updateContact.Attributes["new_type"] = type;
                                service.Update(updateContact);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidPluginExecutionException("invalid plugin "+ex.Message); 
                            }
                        }

                    }
                    #endregion

                    #region
                    if ((updateAccount.Attributes.Contains("new_price")) && (updateAccount.Attributes["new_price"] != null))
                    {
                         Money price = (Money)updateAccount.Attributes["new_price"];
                         if((updateAccount.Attributes.Contains("primarycontactid")) &&(updateAccount.Attributes["primarycontactid"])!=null)
                        {
                            try
                            {
                                EntityReference contactId = (EntityReference)updateAccount.Attributes["primarycontactid"];
                                updateContact.Id = contactId.Id;
                                updateContact.Attributes["new_price"] = price;
                                service.Update(updateContact);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidPluginExecutionException("invalid plugin "+ex.Message); 
                            }
                        }
                    }
                    #endregion
                    
                    #region
                    if((updateAccount.Attributes.Contains("new_numberofdays")) && (updateAccount.Attributes["new_numberofdays"] != null))
                    {
                         Decimal numberofdays = (Decimal)updateAccount.Attributes["new_numberofdays"];
                         if((updateAccount.Attributes.Contains("primarycontactid")) &&(updateAccount.Attributes["primarycontactid"])!=null)
                        {
                            try
                            {
                                EntityReference contactId = (EntityReference)updateAccount.Attributes["primarycontactid"];
                                updateContact.Id = contactId.Id;
                                updateContact.Attributes["new_numberofdays"] = numberofdays;
                                service.Update(updateContact);
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidPluginExecutionException("invalid plugin "+ex.Message); 
                            }
                        }
                    }
                    #endregion

                    #region
                    if ((updateAccount.Attributes.Contains("new_startfrom")) && (updateAccount.Attributes["new_startfrom"] != null))
                   {
                       int startfrom = (int)updateAccount.Attributes["new_startfrom"];
                       if ((updateAccount.Attributes.Contains("primarycontactid")) && (updateAccount.Attributes["primarycontactid"]) != null)
                       {
                           try
                           {
                               EntityReference contactId = (EntityReference)updateAccount.Attributes["primarycontactid"];
                               updateContact.Id = contactId.Id;
                               updateContact.Attributes["new_startfrom"] = startfrom;
                               service.Update(updateContact);
                           }
                           catch (Exception ex)
                           {
                               throw new InvalidPluginExecutionException("invalid plugin " + ex.Message);
                           }
                       }
                   }
                    #endregion
                }
            }
        }
    }
}
