/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html','./src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        bg: '#0f1115',
        panel: '#171a21',
        panel2: '#1d2230',
        text: '#e6eaf2',
        muted: '#9aa4b2',
        free: '#8ef0cf',
        busy: '#ff6b6b',
        accent: '#50c8ff'
      }
    }
  },
  plugins: []
}
