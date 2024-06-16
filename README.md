# Order aggregator

## Configuration

- configuration stored in `appsettings.{development}.json`
  - Order dispatcher timer - sets order dispatcher timer in seconds, default value 20 seconds if not set.
  ```json
  "OrderDispatcher": {
    "DispatcherTimerSeconds": 3
  }
  ```