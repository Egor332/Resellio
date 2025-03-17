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
