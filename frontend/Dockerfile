# ========================
# Build Stage
# ========================
FROM node:20-alpine AS builder

RUN npm install -g pnpm@10.29.1

WORKDIR /app

ARG VITE_API_BASE_URL=/api
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}

COPY package.json pnpm-lock.yaml ./
RUN pnpm install --frozen-lockfile

COPY . .
RUN pnpm run build

# ========================
# Production Stage
# ========================
FROM nginx:stable-alpine

# Remove default config
RUN rm -f /etc/nginx/conf.d/default.conf

# Copy build files
COPY --from=builder /app/dist /usr/share/nginx/html

# Copy nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
