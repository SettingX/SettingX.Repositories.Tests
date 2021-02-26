# SettingX.Repositories.Tests
## About
This solution contains unit/integration tests helpful for developing DB layer implementations. Since the DB layer is deliberately separated from the API/Service layer, it might be a bit troublesome to test each and every small change in DB implementation to make the contracts and basic functionality were not broken. This solution is intended to take care of that - it allows for easier fixes and development of existing and new DB implementations. It runs simple CRUD type operations with dummy data, therefore **do not run them on production environment** as data integrity is not guaranteed!

Currently contains following projects:
1. `SettingX.Repositories.Tests.Rest` - for testing REST HTTP implementation.
## Prerequisites
The solution runs on `dotnet core 3.1` and uses `XUnit` as the testing framework.
## Usage
1. Clone the repository
```sh
    git clone https://github.com/SettingX/SettingX.Repositories.Tests.git
   ``` 
 2. Go to the project directory
 ```sh
    cd SettingX.Repositories.Tests.Rest
   ```
2. Configure environment variables: `SERVICE_URL` indicating the to-be-tested DB implementation endpoint and `DEDICATED_DB` indicating whether or not the instance of the DB implementation was set up specifically for these unit tests and the database itself is, therefore, empty. This will enable additional tests which will be possible only on a clean, unused environment. If not set explicitly, `DEDICATED_DB` defaults to `false`
```sh
    SERVICE_URL=http://localhost:5000
    DEDICATED_DB=true
   ```
3. Run
```sh
    dotnet test
   ```

## License
Distributed under the MIT License. See `LICENSE` for more information.
