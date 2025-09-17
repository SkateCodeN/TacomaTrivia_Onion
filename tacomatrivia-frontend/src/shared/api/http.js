import axios from 'axios';

// In dev: use '/api' so Vite proxy forwards to 5141
// In prod: you can keep reverse-proxying '/api', or set VITE_API_BASE_URL
const baseURL = import.meta.env.DEV
  ? '/api'
  : (import.meta.env.VITE_API_BASE_URL || '/api');

export const api = axios.create({
  baseURL,
  withCredentials: true,
  headers: { 'Content-Type': 'application/json' },
});

api.interceptors.request.use((cfg) => {
  const token = localStorage.getItem('auth.token');
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  cfg.headers['Cache-Control'] = 'no-cache';
  if (cfg.method?.toLowerCase() === 'get') {
    cfg.params = { ...(cfg.params || {}), _ts: Date.now() }; // cache-buster
  }
  return cfg;
});

api.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err?.response?.status === 401) {
      // later: redirect/refresh
    }
    return Promise.reject(err);
  }
);

export const http = {
  get: (url, params) => api.get(url, { params }).then((r) => r.data),
  post: (url, body) => api.post(url, body).then((r) => r.data),
  put:  (url, body) => api.put(url, body).then((r) => r.data),
  del:  (url)       => api.delete(url).then((r) => r.data),
};
