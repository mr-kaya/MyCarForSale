using System.Linq.Expressions;

namespace MyCarForSale.Core.Services;

public interface IGenericService<T> where T : class
{     
    /*
     * IGenericRepository.cs ile aynıdır. Ancak bazı temel değişiklikleri vardır.
     * IGenericRepository.cs memory deki değişikliler ile ilgilenir.
     * IGenericService.cs Database deki değişiklikler ile ilgilenir.
     * Burada delete ve update fonksiyonları Async ifadelerdir.
     */
    
    //Database deki temel CRUD operasyonları burada gerçekleştirilir. "Create",   "Update",   "Delete",   "Read"
    //Not: Task ifadeleri birer Async işlemlerdir. Oluşturacağımız fonksiyonlarda kafamızın karışmaması için, fonksiyonun ismine Async ifadesi eklenir.
    
    //Tablodaki bütün verilerin çekilmesini sağlamak için.
    //IQueryable<T> GetAll(Expression<Func<T, bool>> expression);
    //Yukarıdaki kısım IGenericRepository den. Böyle kullanmamamızın sebebi, bir farklılık olsun diye. Yoksa yukarıdaki kod da iş görür.
    Task<IEnumerable<T>> GetAllAsyncTask();
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
    Task UpdateAsyncTask(T entity);
    //Database Delete işlemi
    Task DeleteAsyncTask(T entity);
    //Database liste halinde Delete işlemi
    Task DeleteRangeAsyncTask(IEnumerable<T> entityEnumerable);
}