namespace EntityFramework_Slider.Helpers
{
    public class Paginate<T>
    {
        public List<T> Datas{ get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }

        public Paginate(List<T> datas, int currentPage, int totalPage)
        {
            Datas= datas;
            CurrentPage = currentPage;
            TotalPage = totalPage;
        }

        public bool HasPrevious
        {
            get
            {
                return CurrentPage > 1;  //hal-hazirda oldugumuz page 1den boyukdurse true olsun yeni gorsensin false halinda disable olsun
            }
        }

        public bool HasNext
        {
            get
            {
                return CurrentPage < TotalPage;  //oldugumuz page totalpagenin sayindan kichikdirse true olsun, eks halda disable olsun
            }
        }

    }
}
