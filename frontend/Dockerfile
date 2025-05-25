# Use official Node.js image
FROM node:22

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install --frozen-lockfile

# Copy all project files
COPY . .

# Expose the Vite default port
EXPOSE 5173

# Start the application with hot reloading
CMD ["npm", "run", "dev", "--", "--host"]
