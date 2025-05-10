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

# Set environment variables inside the container
ENV VITE_API_URL=$VITE_API_URL
ENV VITE_HOST=$VITE_HOST

# Start the application with hot reloading - removing the --watch flag
CMD ["npm", "run", "dev", "--", "--host"]
