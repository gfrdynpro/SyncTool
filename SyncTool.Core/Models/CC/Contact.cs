using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class Contact
{
    [JsonProperty("contact_id")]
    public Guid ContactId
    {
        get; set;
    }

    [JsonProperty("email_address")]
    public EmailAddress EmailAddress
    {
        get; set;
    }

    [JsonProperty("first_name")]
    public string FirstName
    {
        get; set;
    }

    [JsonProperty("last_name")]
    public string LastName
    {
        get; set;
    }

    [JsonProperty("job_title")]
    public string JobTitle
    {
        get; set;
    }

    [JsonProperty("company_name")]
    public string CompanyName
    {
        get; set;
    }

    [JsonProperty("birthday_month")]
    public long BirthdayMonth
    {
        get; set;
    }

    [JsonProperty("birthday_day")]
    public long BirthdayDay
    {
        get; set;
    }

    [JsonProperty("anniversary")]
    public DateTimeOffset Anniversary
    {
        get; set;
    }

    [JsonProperty("update_source")]
    public string UpdateSource
    {
        get; set;
    }

    [JsonProperty("create_source")]
    public string CreateSource
    {
        get; set;
    }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt
    {
        get; set;
    }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt
    {
        get; set;
    }

    [JsonProperty("deleted_at")]
    public DateTimeOffset DeletedAt
    {
        get; set;
    }

    [JsonProperty("custom_fields")]
    public CustomField[] CustomFields
    {
        get; set;
    }

    [JsonProperty("phone_numbers")]
    public PhoneNumber[] PhoneNumbers
    {
        get; set;
    }

    [JsonProperty("street_addresses")]
    public StreetAddress[] StreetAddresses
    {
        get; set;
    }

    [JsonProperty("list_memberships")]
    public Guid[] ListMemberships
    {
        get; set;
    }

    [JsonProperty("taggings")]
    public Guid[] Taggings
    {
        get; set;
    }

    [JsonProperty("notes")]
    public Note[] Notes
    {
        get; set;
    }
}
