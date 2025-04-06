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

# Running frontend in development
The most convenient way is to run all other dependencies (backend, db etc.) through docker.

1. Run docker-compose (use `--build` flag to ensure the newest code is used and `-d` to run in detached mode)
    ```sh
    docker-compose up --build -d
    
2. Run frontend locally on different port than docker exposes it (currently 5173):
    ```sh
    npm run dev -- --port=3000

3. Stop docker-compose:
   ```sh
   docker-compose down
