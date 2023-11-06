using System.Linq.Expressions;

namespace MyCarForSale.Core.Repositories;

public interface IGenericRepository<T> where T : class
{
    //Temel CRUD operasyonları burada gerçekleştirilir. "Create",   "Update",   "Delete",   "Read"
    //Not: Task ifadeleri birer Async işlemlerdir. Oluşturacağımız fonksiyonlarda kafamızın karışmaması için, fonksiyonun ismine Async ifadesi eklenir.
    //Tablodaki bütün verilerin çekilmesini sağlamak için.
    IQueryable<T> GetAll();
    
    //Id ye göre sorgulama
    Task<T> GetByIdAsyncTask(int id);
    /*
     * Task(Görev) Bu ifade ASYNC(Asenkron) bir işlemdir.
     * Yani, program başka işlemler yaparken bu işlemi de yapmaya devam eder.
     */
    
    //Kendi belirlediğimiz kurallara göre sorgulama ve bunun karşılığında da verilerin liste olarak dönmesi.
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    /*
     * IQueryable ifadesi, bir liste tutar.
     * Burada List<> değilde IQueryable<> kullanmamızın sebebi;
     * veriler database e gitmeden istenilen verileri aratma(where), sıralama(order by) kullanarak direkt istediğimiz veriyi getirtebilmek.
     *
     * Expression: EF Core da sorguların hepsi bir expression alır.
     * Func: Bu default gelen bir delege dir. <Func> <Action> <Predicate> <EventHandler>
     *
     * Örnek ifadesi: carFeaturesRepository.where(x => x.id > 5).OrderBy.ToListAsync();
     */

    //Direkt veriyi değil de, verinin olup olmadığını döner. Veri varsa True, yoksa False.
    Task<bool> AnyAsyncTask(Expression<Func<T, bool>> expression);
    
    //Tabloya veri ekleme.
    Task AddAsyncTask(T entity);

    //Tabloya liste halinde veri ekleme.
    Task AddRangeAsyncTask(IEnumerable<T> entityEnumerable);

    //Database Update işlemi
    void Update(T entity);
    //Database Delete işlemi
    void Delete(T entity);
    //Database liste halinde Delete işlemi
    void DeleteRange(IEnumerable<T> entityEnumerable);
    /*
     * Update ve Delete işlemlerinde Async kullanılmaz.
     * Çünkü, .net core update veya delete işlemlerinde veriyi memory e çeker.
     * Bu sayede herhangi bir bekleme veya yoğun bir işlem olmamış olur.
     */
}