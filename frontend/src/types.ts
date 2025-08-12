export type TableStatus = { tableId: number; isReserved: boolean };
export type Reservation = {
  id?: number;
  tableId: number;
  startAt: string; // ISO
  durationMinutes: number;
  customerName: string;
  phone?: string | null;
  partySize?: number | null;
  note?: string | null;
};
