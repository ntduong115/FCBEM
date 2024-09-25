using Model.PaperModels;
using Model;
using static Core.Commons.FCConstants;

namespace FCCore.DataAccess
{
    public static class PapersDAL
    {
        public static List<PaperNew> GetMostViewedPaperNews(this DatabaseContext context, int number)
        {
            DateTime timeNow = DateTime.Now;

            DateTime currentTime = new(timeNow.Year, timeNow.Month, 1);
            DateTime timeStart = currentTime.AddMonths(-1);
            DateTime timeEnd = currentTime.AddMonths(1);

            var ars = context.PaperNews.Where(a => a.CreatedDate > timeStart &&
                                         a.CreatedDate < timeEnd &&
                                         (a.Status == ArticleStatus.Show))
                                    .OrderByDescending(a => a.UpdatedDate)
                                    .Take(number);

            var listData = ars.ToList();

            return listData;
        }
        public static List<PaperNew> GetViewedPaperNews(this DatabaseContext context)
        {
            DateTime timeNow = DateTime.Now;
            DateTime currentTime = new(timeNow.Year, timeNow.Month, 1);
            DateTime timeStart = currentTime.AddMonths(-1);
            DateTime timeEnd = currentTime.AddMonths(1);
            var ars = context.PaperNews.Where(a => a.CreatedDate > timeStart &&
                                                 a.CreatedDate < timeEnd &&
                                                 (a.Status == ArticleStatus.Show
                                                 )
                                            ).OrderByDescending(a => a.CreatedDate);

            var listData = ars.ToList();

            return listData;
        }
    }
}
