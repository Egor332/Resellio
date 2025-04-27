# Project Setup

1. Clone repository:
    ```sh
    git clone https://github.com/Egor332/Resellio
    
2. Set up environment variables:
    Create an appsettings.Development.json file in the /backend/ResellioBackend and in the /backend/NotificaitonBackedn of the project, following the structure of appsettings.json found in corresponding direcotries.


3. Run project via docker:  
    ```sh
    docker compose up -d
    docker-compose exec backend dotnet ef database update --project ResellioBackend    
# Frontend Setup

1. Install nvm:
    ```sh
    curl -fsSL https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.4/install.sh | bash
    echo 'export NVM_DIR="$HOME/.nvm"' >> ~/.bashrc
    echo '[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"' >> ~/.bashrc
    source ~/.bashrc

2. Install npm (v22):
    ```sh
    nvm install 22
    nvm use 22
    
3. Install dependencies:  
    ```sh
    npm install

# Frontend formatting
The project uses Prettier as a formatter, to format all files use:
```sh
npm run format
```

# Running frontend in development
The most convenient way is to run all other dependencies (backend, db etc.) through docker.

1. Run docker-compose (use `--build` flag to ensure the newest code is used and `-d` to run in detached mode) in project root folder:
    ```sh
    docker-compose up --build -d
    
2. Run frontend locally on different port than docker exposes it (currently 5173) in `/frontend` folder:
    ```sh
    npm run dev -- --port=3000

3. Stop docker-compose:
   ```sh
   docker-compose down

env.development is no longer stored on remote repo, here is its scheme:
```
# Base URL for the API
VITE_API_URL = '<API base URL>'

# Hostname for the application
VITE_HOST = '<application hostname>'

# Port for the frontend development server
VITE_FRONTEND_PORT = <frontend server port>

# Default HTTP timeout in milliseconds
VITE_DEFAULT_HTTP_TIMEOUT = <timeout in ms>
```

# Access MSSQL database running in Docker:
In appropriate container run this:
```sh
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P Pass@word -N -C
```

Ensure in docker-compose db service contains the following env variables:
```yaml
- MSSQL_ENCRYPT=OPTIONAL
- MSSQL_TRUST_CERT=ON
```


