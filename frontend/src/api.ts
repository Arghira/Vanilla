import type { TableStatus, Reservation } from './types';

const API_BASE = import.meta.env.VITE_API_BASE || window.location.origin;

export async function getStatus(startAtIso: string): Promise<TableStatus[]> {
  const r = await fetch(`${API_BASE}/status?startAt=${encodeURIComponent(startAtIso)}`);
  if (!r.ok) throw new Error('Failed to load status');
  return r.json();
}

export async function createReservation(data: Reservation): Promise<number> {
  const r = await fetch(`${API_BASE}/reservations`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  });
  if (r.status === 201) {
    const body = await r.json().catch(() => ({}));
    return body.id ?? 0;
  }
  if (r.status === 409) throw new Error('Masa tocmai a fost rezervată.');
  const txt = await r.text();
  throw new Error(txt || `Eroare: ${r.status}`);
}
