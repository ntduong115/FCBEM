using Core.Models.Utility;

using static Core.Commons.FCBEMConstants;

namespace Core.Commons
{
    public static class FCBEMConstantsHelpers
    {
        public static List<SelectListModel> PositionSelectList =
        [
            new(){ Value = null, Name = "Please choose"},
            new(){ Value = Position.Prof, Name = "Prof."},
            new(){ Value = Position.AssociateProf, Name = "Assistant Prof."},
            new(){ Value = Position.AssistantProf, Name = "Associate Prof."},
            new(){ Value = Position.Dr, Name = "Dr."},
            new(){ Value = Position.Master, Name = "Master"},
            new(){ Value = Position.SeniorLecturer, Name = "Senior Lecturer"},
            new(){ Value = Position.Lecturer, Name = "Lecturer"},
            new(){ Value = Position.PhDstudent, Name = "Ph.D student"},
            new(){ Value = Position.MasterStudent, Name = "Master student"},
            new(){ Value = Position.UndergraduateStudent, Name = "Undergraduate student"},
            new(){ Value = Position.Others, Name = "Others"},
        ];
        public static List<SelectListModel> PresentationTypeSelectList =
        [
            new(){ Value = null, Name = "Please choose"},
            new(){ Value = PresentationType.OralPresentation, Name = "Oral Presentation"},
            new(){ Value = PresentationType.NoSubmission, Name = "No submission"},
        ];
        public static List<SelectListModel> RegistrationTypeSelectList =
        [
            //new(){ Value = null, Name = "Please choose"},
            new(){ Value = RegistrationType.Author, Name = "Author"},
            new(){ Value = RegistrationType.Listener, Name = "Listener"},
        ];
        public static List<SelectListModel> DietTypeSelectList =
        [
            //new(){ Value = null, Name = "Please choose"},
            new(){ Value = DietType.RegularDiet, Name = "Regular diet"},
            new(){ Value = DietType.VegetarianDiet, Name = "Vegetarian diet"},
            new(){ Value = DietType.VeganDiet, Name = "Vegan diet"},
            new(){ Value = DietType.KosherDiet, Name = "Kosher diet"},
            new(){ Value = DietType.HalalDiet, Name = "Halal diet"},
            new(){ Value = DietType.OtherDiet, Name = "Other diet ( Please specify in comments)"},
        ];
        public static List<SelectListModel> SubmissionTypeSelectList =
        [
            new(){ Value = SubmissionType.AbstractPO, Name = "Abstract (Presentation Only)"},
            new(){ Value = SubmissionType.AbstractPAPP, Name = "Abstract (Presentation and Possible Publication)"},
            new(){ Value = SubmissionType.FullPaper, Name = "Full Paper (Presentation and Possible Publication)"},
        ];
        public static List<SelectListModel> PaperStatusSelectList =
        [
            new(){  Name = "All"},
            new(){ Value = PaperStatus.Pending, Name = "Pending"},
            new(){ Value = PaperStatus.Accepted, Name = "Accepted"},
            new(){ Value = PaperStatus.Reject, Name = "Reject"},
        ];
    }
}