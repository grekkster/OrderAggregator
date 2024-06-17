# Order aggregator

## Pozn�mky

- Nejsem si jist�, jestli jsem zad�n� pochopil spr�vn�, mo�n� to bylo my�leno tak, �e slu�ba po dobu 20 vte�in sb�r� objedn�vky, bude je n�jak efeketivn� ukl�dat a za 20 vte�in je ode�le intern�mu syst�mu a sb�r� nov�, asi by to d�valo v�t�� smysl, nicm�n� m� to napadlo ke konci implementace, pokud by to tak bylo, v���m �e by se implementace dala snadno p�ed�lat. V sou�asn� dob� slu�ba funguje tak, �e sb�r� a agreguje objedn�vky, pos�l� je intern�mu syst�mu (konzole), ale nevyprazd�uje.
- Pokud by to bylo mo�n� r�d bych od V�s dostal zp�tnou vazbu na proveden� projektu, co by se dalo ud�lat l�pe, jak� je optim�ln� �e�en�. R�d se v t�hle problematice pou��m.

## Konfigurace

- konfigurace je ulo�ena v `appsettings.{development}.json`
  - Order dispatcher timer - nastav� �asova� odbaven� objedn�vek, defaultn� hodnota 20s, pokud nen� nastaveno.
  ```json
  "OrderDispatcher": {
    "DispatcherTimerSeconds": 20
  }
  ```

## Mo�n� upgrady

- Ur�it� bych p�idal n�jak� z�t�ov� testy. Na z�klad� nich, p��padn� n�jak� anal�zy v�konu a hled�n� bottleneck� bych navrhl p��padn� upgrady, opravy, kdy� bych m�l lep�� p�edstavu, kde mohou nastat probl�my.
- Upgrady, kter� m� napadaj� ihned:   
  - Pou��t ke�ov�n� a vhodn� ulo�i�t� pro �ast� update s relativn� mal�mi daty, ke�ov�n� v pam�ti , p��padn� n�jak� robustn�j�� rychl� a spolehliv� ulo�i�t� (Redis).
  - Logov�n�, glob�ln� zpracov�n� v�jimek.
  - Batch processing - pokud by byl probl�m s �etnost� updat� a nevadila by prodleva, pak bych je sb�ral do z�sobn�ku a z n�j p�i napln�n� updatoval najednou do ulo�i�t�.
  - Je nast�n�no viz `BatchStorage.cs` a `OrderService.AddToUpdateBatch(..)`.
  - Pou�it� asynchronn�ho programov�n� pro soub�n� a neblokovan� zpracov�n� objedn�vek.
  - Pos�lit odolnost aplikace - retry pattern, timeouty atp.
  - Pokud by bylo vhodn�, updaty p�ed�vat n�jak�mu messaging syst�mu a asynchronn� zpracov�vat pomoc� dal��ch slu�eb.
  - P��padn� pou��vat `BackgroundService` bu� na update ke� se kterou by se pracovalo, nebo naopak na aktualizaci persistentn�ho ulo�i�t�.
  - M�sto batchov�n� updat� pou��t n�jakou formu fronty s asynchronn�m zpracov�n�m updat�.
  - Pokud by bylo t�eba, nastavit limit na po�et paraleln� zpracov�van�ch �loh.