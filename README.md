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
