# Build stage
FROM node:20-alpine AS builder

# Install pnpm
RUN npm install -g pnpm@10.29.1

WORKDIR /app

# Accept build argument
ARG VITE_API_BASE_URL=http://localhost:5150/api

# Set as environment variable for Vite
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}

# Debug: Show the API URL being used
RUN echo "Building with VITE_API_BASE_URL=${VITE_API_BASE_URL}"

# Copy package files
COPY package.json pnpm-lock.yaml ./

# Install dependencies
RUN pnpm install --frozen-lockfile

# Copy source code
COPY . .

# Build app (Vite will use VITE_API_BASE_URL from ENV)
RUN pnpm run build

# Production stage
FROM nginx:alpine

# Copy built files
COPY --from=builder /app/dist /usr/share/nginx/html

# Copy nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
