namespace MyCarForSale.Core.UnitOfWorks;

/*
 *  Bu interface in oluşma sebebi;
 *  Her bir repository çağrıldığında ardından SaveChangeAsync metodu yada SaveChange metodu çağrılmalıdır.
 *  Ancak bu bazen, database e eksik veri gitmesine sebep olabilir.
 *  Bunu engellemek için, ne zaman istersek, o zaman SaveChange ifadesini çalıştırabiliriz. 
 */

public interface IUnitOfWork
{
    Task CommitAsyncTask(); //SaveChangeAsync metoduna karşılık gelir.
    void Commit(); //SaveChange metoduna karşılık gelir.
}