namespace RPM.Common
{
    using System;

    public static class GlobalConstants
    {
        public const string SystemName = "Homy";
        public const string HomySupportEmail = "dotnetdari@gmail.com";

        public const string AdministratorRoleName = "Admin";
        public const string OwnerRoleName = "Owner";
        public const string ManagerRoleName = "Manager";
        public const string ViewerRoleName = "Viewer";
        public const string TenantRole = "Tenant";
        public const string MessagesAuthorizedRoles = "Owner,Manager,Tenant";

        public const string AdminArea = "Administration";
        public const string ManagementArea = "Management";

        public const string AdminUserName = "Admin";
        public const string OwnerUserName = "Owner";
        public const string ManagerUserName = "Manager";

        public const string AdminEmail = "admin@admin.com";
        public const string OwnerEmail = "owner@owner.com";
        public const string ManagerEmail = "manager@manager.com";

        public const string DefaultAdminPassword = "adminA123";
        public const string DefaultOwnerPassword = "owner123";
        public const string DefaultManagerPassword = "manager123";

        public const string AdminFirstName = "Admin4o";
        public const string OwnerFirstName = "Domolin";
        public const string ManagerFirstName = "Kanalin";

        public const string AdminLastName = "Adminski";
        public const string OwnerLastName = "Barakov";
        public const string ManagerLastName = "Tsolov";

        // Input Models Constraints
        public const int RegisterPasswordMin = 2;
        public const int RegisterPasswordMax = 50;

        public const int MessageMinLength = 10;
        public const int MessageMaxLength = 500;

        public const string ManagerFeeMin = "1";
        public const string ManagerFeeMax = "100000";

        // Entities Property Constraints
        public const int UsersNameMin = 2;
        public const int UsersNameMax = 50;

        public const int AboutMaxLength = 200;
        public const int ContractTitleMaxLength = 30;

        public const int HomeNameMin = 5;
        public const int HomeNameMax = 40;

        public const int HomeDescriptionMin = 20;
        public const int HomeDescriptionMax = 1000;

        public const int HomeAddressMin = 10;
        public const int HomeAddressMax = 100;

        public const string PriceMin = "10";
        public const string PriceMax = "1000000";

        public const int RequestDocumentMaxSize = 2 * 1024 * 1024;
        public const int ContractDocumentMaxSize = 4 * 1024 * 1024;

        public const int CountryCodeLength = 2;

        public const string RegexAddress = @"^[\d-]{1,4}, [A-Za-z-. ]+, [\d]+";
        public const string RegexAddressError = "Enter address in the format: number, street name, post code";

        public const int ReasonMaxLength = 30;

        // TempData and Messages
        public const string TempDataSuccessKey = "SuccessMessage";
        public const string TempDataErrorKey = "ErrorMessage";
        public const string TempDataInfoKey = "InfoMessage";

        public const string NoLuckMan = "Didn't work out as you thought.";
        public const string NiceTry = "Nice try, smartass!";

        public const string InvalidIdentityError = "Invalid identity details";
        public const string CouldNotDeleteRecord = "Error: record was not deleted";
        public const string CouldNotUpdateRecord = "Error: record was not updated";
        public const string CouldNotCreateRecord = "Error: record was not created";
        public const string DeltionDenied = "You cannot delete the city, because there are registered listings in it.";
        public const string CouldNotFind = "No results found.";
        public const string MissingId = "Missing entity Id.";
        public const string RecordIdIsInvalid = "Invalid record id";
        public const string YouMustBeLoggedIn = "You must be logged in";
        public const string EmptyFields = "Please fill the required fields";
        public const string EmptyField = "Please fill the required field";
        public const string EmptyMessage = "The message is empty";
        public const string InvalidEntryData = "Invalid input data";
        public const string FileTooLarge = "The file is larger than 2MB";
        public const string TitleTooLong = "The title must be less than {1} characters long";
        public const string UserNotFound = "Requesting user not found in database";
        public const string EntityNotFound = "Entity not found";

        public const string RecordDeletedSuccessfully = "Successfully deleted record.";
        public const string RecordCreatedSuccessfully = "Successfully created record.";
        public const string RecordUpdatedSuccessfully = "Successfully updated record.";
        public const string SuccessfullySubmittedRequest = "You have successfully submitted your request";
        public const string FailedToSubmitRequest = "You have failed to submit your request. Contact support";
        public const string RecordAlreadyExists = "Record already exists.";
        public const string RentCreatedSuccessfully = "Successfully initiated your rent";
        public const string TenantRemovedSuccessfully = "Successfully removed tenant of your home";
        public const string ManagerAddedSuccessfully = "Successfully added manager {0} to your home";
        public const string ManagerRemovedSuccessfully = "Successfully removed manager {0} from your home";
        public const string RequestRejectedSuccessfully = "Request rejected successfully";

        public const string RemovedSuccessfully = "Your listing was removed successfully";
        public const string NotAllowedToRemove = "Your listing can't be removed. It is rented and/or managed.";

        public const string UserAddedToRole = "User {0} added to {1} role!";
        public const string SuccessfullyRegistered = "You have registered successfully to Stripe";
        public const string PaymentSuccessfull = "Thank you! Your payment was successfull!";
        public const string PaymentFailed = "Payment failed! Please contact support.";
        public const string PaymentSuccessfulButStatusNotUpdated = "Thank you! Your payment was successfull! However, the status was not updated. Please contact support.";

        public const string ManagerHasNotStripeAccount =
            "Your manager {0} doesn't have a Stripe account. Please let him/her register Stripe account through Homy in order to receive funds from you.";

        public const string MessageSentSuccessfully = "Thank you! Your message was sent. We'll get in touch soon.";

        // DisplayNames
        public const string DisplayNameCountryCode = "Country Code";

        // Images
        public const string CloudFolder = "Homy";
        public const string PubPrefix = "dotnethomy";
        public const string GetImageUrlFormat = "{0}.jpg";
        public const string Limit = "limit";
        public const string Fill = "fill";
        public const string Thumb = "thumb";
        public const string GravityFaces = "faces";
        public const string RadiusMax = "max";
        public const int ImgWidth = 800;
        public const int ImgHeight = 600;
        public const string DefaultPropertyImage = "https://res.cloudinary.com/dotnetdari/image/upload/v1584364747/Homy/Default_House_Image_vdhzul.png";

        public const string HouseImage =
           "https://res.cloudinary.com/dotnetdari/image/upload/v1587205946/Homy/house_1_ovyvnj.jpg";
        public const string ApartmentImage =
            "https://res.cloudinary.com/dotnetdari/image/upload/v1587205946/Homy/apartment_1_x4figf.jpg";
        public const string RoomImage =
            "https://res.cloudinary.com/dotnetdari/image/upload/v1587205946/Homy/room_1_xeu9wy.jpg";

        // Dates
        public const string StartDate = "1/1/1920";
        public const string StandartDateFormat = "dd/MM/yyyy";
        public const string DateFormatWithTime = "dd/MM/yyyy h:mm tt";

        // Paging
        public const int PageSize = 10;

        // Action Names
        public const string AddToRole = "AddToRole";
        public const string RemoveRole = "RemoveFromRole";

        // Enum values
        public const string Managed = "Managed";
        public const string Rented = "Rented";
        public const string ToRent = "ToRent";
        public const string ToManage = "ToManage";
        public const string CancelRent = "CancelRent";
        public const string CancelManage = "CancelManage";
        public const string NA = "NA";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";

        public const string House = "House";
        public const string Apartment = "Apartment";
        public const string Room = "Room";

        public const int ShortStringLength = 30;

        // ViewModel Constants
        public const string DashboardRequestFullName = "{0} {1}";
        public const string DashboardRequestLocation = "{0}, {1}";

        public const string DashboardRentalFullName = "{0} {1}";
        public const string DashboardRentalLocation = "{0}";

        public const string TenantFullName = "{0} {1}";
        public const string ManagerFullName = "{0} {1}";
        public const string OwnerFullName = "{0} {1}";
        public const string RecipientFullName = "{0} {1}";
        public const string RentalLocation = "{0} | {1} | {2}";
        public const string PaymentRentalLocation = "{0} | {1}";
        public const string HomeLocation = "{0} | {1} | {2}";

        public const string MessageLengthError = "The {0} must be at least {2} and at max {1} characters long.";
        public const string NameLengthError = "The {0} must be at least {2} and at max {1} characters long.";

        // Stripe Constants
        public const string HomyTestSecretKey = "sk_test_EefMtEHdBjJZxXFABAUxJnOv00rjqjdoSS";
        public const string HomyTestPublishableKey = "pk_test_AzKPy7vAIagF2a0AFgfCmKiQ00W01hYiRX";
        public const string HomyStrypeTestModeClientId = "ca_GzhpVHTCtG6gcug43QcvIjNxyMoVooBi";
        public const string HomyStrypeTestModeClientIdConnectLink =
            "https://connect.stripe.com/oauth/authorize?response_type=code&client_id=" + "ca_HomyStrypeTestModeClientId" + "&scope=read_write";

        public const string HomyConnectLinkForManagerWithRedirect =
            "https://connect.stripe.com/oauth/authorize?response_type=code&client_id=" + "ca_HomyStrypeTestModeClientId" + "&scope=read_write" + "&redirect_uri=https://localhost:44319/Stripe/StripeCallback";

        public const string HomyConnectLinkForWithRedirect =
            "https://connect.stripe.com/oauth/authorize?response_type=code&client_id=" + "ca_HomyStrypeTestModeClientId" + "&scope=read_write" + "&redirect_uri=https://localhost:44319/Stripe/StripeCallback";

        public const string CheckoutIdDoesNotExist = "The Stripe CheckoutId does not exist! Contact the Homy Support.";

        // Sendgrid
        public const string SendGridKey = "SendGridKey";

        // Hangfire
        public const string DashboardRoute = "/hangfire";
        public const string HangfireEmail = "HangfireEmail";

        // Google ReCAPTCHA Constants
        public const string ReCaptchaSiteKey = "ReCaptchaSiteKey";
        public const string ReCaptchaSecretKey = "ReCaptchaSecretKey";
        public const string ReCaptchaVerifyLink = "https://www.google.com/recaptcha/api/siteverify";

        public const string CurrencyUSD = "usd";
        public const string CurrencyEUR = "eur";

        public const string ResponseStatusSucceeded = "Succeeded";

        // External Authentication
        public const string FaceBookAppId = "FaceBookAppId";
        public const string FaceBookAppSecret = "FaceBookAppSecret";
        public const string GoogleClientId = "GoogleClientId";
        public const string GoogleClientSecret = "GoogleClientSecret";

        // Custom Cron Expressions
        public const string TenMinute = "*/10 * * * *";
        public const string MonthlyOneDayBeforeEoM = "0 0 {0} * *";
    }
}
