import { useEffect, useRef } from 'react';
import type { Reservation } from '../types';

export default function ReservationDialog({
  open,
  onClose,
  data,
  onConfirm,
}: {
  open: boolean;
  data: { tableId: number; whenHuman: string; startAtIso: string; duration: number } | null;
  onClose: () => void;
  onConfirm: (payload: Reservation) => Promise<void>;
}) {
  const dialogRef = useRef<HTMLDialogElement>(null);

  useEffect(() => {
    const d = dialogRef.current!;
    if (open) d.showModal(); else d.close();
  }, [open]);

  if (!data) return null;

  const handle = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const form = e.currentTarget as HTMLFormElement & {
      name: { value: string };
      phone: { value: string };
      party: { value: string };
      note: { value: string };
      dur: { value: string };
    };
    await onConfirm({
      tableId: data.tableId,
      startAt: data.startAtIso,
      durationMinutes: Number(form.dur.value || data.duration || 60),
      customerName: form.name.value.trim(),
      phone: form.phone.value.trim() || null,
      partySize: form.party.value ? Number(form.party.value) : null,
      note: form.note.value.trim() || null,
    });
  };

  return (
    <dialog ref={dialogRef} className="dialog">
      <form method="dialog" onSubmit={handle} className="w-full">
        <div className="card-h">
          <strong>Rezervare masă {data.tableId}</strong>
          <button type="button" className="text-muted" onClick={onClose}>×</button>
        </div>
        <div className="card-c space-y-3">
          <div className="text-muted">Interval: {data.whenHuman}</div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            <div>
              <label className="label">Nume</label>
              <input name="name" required placeholder="Nume și prenume" className="input" />
            </div>
            <div>
              <label className="label">Telefon</label>
              <input name="phone" placeholder="07xx xxx xxx" className="input" />
            </div>
            <div>
              <label className="label">Persoane</label>
              <input name="party" type="number" min={1} max={20} defaultValue={2} className="input" />
            </div>
            <div>
              <label className="label">Durata</label>
              <select name="dur" defaultValue={data.duration} className="select">
                <option value="60">60 min</option>
                <option value="90">90 min</option>
                <option value="120">120 min</option>
              </select>
            </div>
            <div className="sm:col-span-2">
              <label className="label">Notă</label>
              <textarea name="note" rows={3} className="textarea" placeholder="Preferințe / ocazie specială"></textarea>
            </div>
          </div>
        </div>
        <div className="card-c border-t border-white/10 flex justify-end gap-2">
          <button type="button" className="btn" onClick={onClose}>Anulează</button>
          <button className="btn btn-primary" value="default">Confirmă rezervarea</button>
        </div>
      </form>
    </dialog>
  );
}
