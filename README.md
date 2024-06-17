# Order aggregator

## Poznámky

- Nejsem si jistý, jestli jsem zadání pochopil správnì, možná to bylo myšleno tak, že služba po dobu 20 vteøin sbírá objednávky, bude je nìjak efeketivnì ukládat a za 20 vteøin je odešle internímu systému a sbírá nové, asi by to dávalo vìtší smysl, nicménì mì to napadlo ke konci implementace, pokud by to tak bylo, vìøím že by se implementace dala snadno pøedìlat. V souèasné dobì služba funguje tak, že sbírá a agreguje objednávky, posílá je internímu systému (konzole), ale nevyprazdòuje.
- Pokud by to bylo možné rád bych od Vás dostal zpìtnou vazbu na provedení projektu, co by se dalo udìlat lépe, jaké je optimální øešení. Rád se v téhle problematice pouèím.

## Konfigurace

- konfigurace je uložena v `appsettings.{development}.json`
  - Order dispatcher timer - nastaví èasovaè odbavená objednávek, defaultní hodnota 20s, pokud není nastaveno.
  ```json
  "OrderDispatcher": {
    "DispatcherTimerSeconds": 20
  }
  ```

## Možné upgrady

- Urèitì bych pøidal nìjaké zátìžové testy. Na základì nich, pøípadnì nìjaké analýzy výkonu a hledání bottleneckù bych navrhl pøípadné upgrady, opravy, když bych mìl lepší pøedstavu, kde mohou nastat problémy.
- Upgrady, které mì napadají ihned:   
  - Použít kešování a vhodné uložištì pro èasté update s relativnì malými daty, kešování v pamìti , pøípadnì nìjaké robustnìjší rychlé a spolehlivé uložištì (Redis).
  - Logování, globální zpracování výjimek.
  - Batch processing - pokud by byl problém s èetností updatù a nevadila by prodleva, pak bych je sbíral do zásobníku a z nìj pøi naplnìní updatoval najednou do uložištì.
  - Je nastínìno viz `BatchStorage.cs` a `OrderService.AddToUpdateBatch(..)`.
  - Použití asynchronního programování pro soubìžné a neblokované zpracování objednávek.
  - Posílit odolnost aplikace - retry pattern, timeouty atp.
  - Pokud by bylo vhodné, updaty pøedávat nìjakému messaging systému a asynchronnì zpracovávat pomocí dalších služeb.
  - Pøípadnì používat `BackgroundService` buï na update keš se kterou by se pracovalo, nebo naopak na aktualizaci persistentního uložištì.
  - Místo batchování updatù použít nìjakou formu fronty s asynchronním zpracováním updatù.
  - Pokud by bylo tøeba, nastavit limit na poèet paralelnì zpracovávaných úloh.