namespace Core.Commons
{
    public static class FCConstants
    {
        public const string SECRETKEY = "OOOxAjodifjasd89wrqw989849890sdf203829384n8as8df9asdfjkn234n2j3n4k3384u234n2k3jn4ji2n34i2u3n4iu23409u48023u94823423n4jk2n5450345u0934u805u68956586094589323jnjkfnsdkjfnsdjfsdjfiosduf90209wioerujgnrktbrnmddaio";


        public enum ArticleStatus
        {
            WaitingApprove = 0,
            Show = 1,
            Hide = 2,
            Delete = 3,
        }

        public enum PaperStatus
        {
            Pending = 0,
            Reject = 1,
            Accepted = 2,
        }

        public enum RegistrationStatus
        {
            Pending = 0,
            Reject = 1,
            Completed = 2,
        }

        public readonly struct PathUpload
        {
            public static readonly string PAPER = "Resources\\Uploads\\Papers";
            public static readonly string NEWS = "Resources\\Uploads\\NEWS";
            public static readonly string NEWS_THUMB = "Resources\\Uploads\\NEWS_Thumb";
            public static readonly string REGISTRAION = "Resources\\Uploads\\Registrations";

            public static readonly string TEMP = "Resources\\TEMP";

            public static readonly string MailImage = "Resources\\Images\\Mail";

        }
        public readonly struct PathSource
        {
            public static readonly string MailImage = "Resources\\Mail\\Images";
            public static readonly string MailContent = "Resources\\Mail\\MailContent";
        }

        public readonly struct FileName
        {
            public static readonly string RegisterSuccess = "register-success.txt";
            public static readonly string ForgetPassword = "forget-password.txt";

            public static readonly string Submission = "submission-success.txt";
            public static readonly string SubmissionSubject = "submission-success-subject.txt";

            public static readonly string AbstractAccept = "abstract-accept.txt";
            public static readonly string AbstractAcceptSubject = "abstract-accept-subject.txt";
            public static readonly string AbstractReject = "abstract-reject.txt";
            public static readonly string AbstractRejectSubject = "abstract-reject-subject.txt";

            public static readonly string FullpaperAccept = "fullpaper-accept.txt";
            public static readonly string FullpaperAcceptSubject = "fullpaper-accept-subject.txt";
            public static readonly string FullpaperReject = "fullpaper-reject.txt";
            public static readonly string FullpaperRejectSubject = "fullpaper-reject-subject.txt";

            public static readonly string RegistrationComplete = "registration-complete.txt";
            public static readonly string RegistrationCompleteSubject = "registration-complete-subject.txt";

            public static readonly string RegistrationSent = "registration-sent.txt";
            public static readonly string RegistrationSentSubject = "registration-sent-subject.txt";

            public static readonly string Image1 = "image1.jpg";
            public static readonly string Image2 = "image2.jpg";
            public static readonly string Image3 = "image3.jpg";
        }

        public static class PolicyConstant
        {
            public const string LoginOnly = "LoginOnly";
        }

        public static class RoleName
        {
            public const string Admin = "Administrator";
            public const string Client = "Client";
        }
        public static class SubmissionType
        {
            public const string FullPaper = "FullPaper";
            public const string AbstractPO = "AbstractPO";
            public const string AbstractPAPP = "AbstractPAPP";
        }
        public enum PresentationType
        {
            OralPresentation = 0,
            NoSubmission = 1,
        }
        public enum Position
        {
            Prof = 0,
            AssociateProf = 1,
            AssistantProf = 2,
            Dr = 3,
            Master = 4,
            SeniorLecturer = 5,
            Lecturer = 6,
            PhDstudent = 7,
            MasterStudent = 8,
            UndergraduateStudent = 9,
            Others = 10,
        }
        public enum RegistrationType
        {
            Author = 0,
            Listener = 1,
        }
        public enum DietType
        {
            RegularDiet = 0,
            VegetarianDiet = 1,
            VeganDiet = 2,
            KosherDiet = 3,
            HalalDiet = 4,
            OtherDiet = 5,
        }
    }
}


