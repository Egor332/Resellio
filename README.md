# Project Setup

1. Clone repository:
    ```sh
    git colone https://github.com/Egor332/Resellio
    
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
    
4. Run app:
    ```sh
    npm run dev
