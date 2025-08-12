import { useEffect, useMemo, useState } from 'react';
import Map from './components/Map';
import ReservationDialog from './components/ReservationDialog';
import { createReservation, getStatus } from './api';

function roundToNextHour(d: Date) {
  const x = new Date(d); x.setMinutes(0,0,0); x.setHours(x.getHours()+1); return x;
}
function toInputDate(d: Date) { return d.toISOString().slice(0,10); }
function toInputTime(d: Date) { return `${String(d.getHours()).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')}`; }

export default function App(){
  const [date, setDate] = useState('');
  const [time, setTime] = useState('');
  const [duration, setDuration] = useState(60);
  const [status, setStatus] = useState([] as Awaited<ReturnType<typeof getStatus>>);
  const [toast, setToast] = useState('');
  const [modal, setModal] = useState<{ tableId: number; whenHuman: string; startAtIso: string; duration: number } | null>(null);

  useEffect(() => {
    const now = roundToNextHour(new Date());
    setDate(toInputDate(now));
    setTime(toInputTime(now));
  }, []);

  const slotIso = useMemo(() => {
    if(!date || !time) return '';
    return new Date(`${date}T${time}:00`).toISOString();
  }, [date, time]);

  async function refresh(){
    if(!slotIso) return;
    try {
      setToast('Se încarcă disponibilitatea…');
      const data = await getStatus(slotIso);
      setStatus(data);
      setToast('Disponibilitate actualizată.');
      setTimeout(()=>setToast(''), 1200);
    } catch {
      setToast('Nu am putut încărca statusul.');
      setTimeout(()=>setToast(''), 1800);
    }
  }

  useEffect(()=>{ refresh(); }, [slotIso]);

  async function handleCreate(payload: Parameters<typeof createReservation>[0]){
    try {
      await createReservation(payload);
      setToast('Rezervare creată!');
      setModal(null);
      refresh();
    } catch(e:any) {
      setToast(e.message || 'Eroare la salvare');
    }
    setTimeout(()=>setToast(''), 1800);
  }

  const whenHuman = useMemo(() => slotIso ? new Date(slotIso).toLocaleString(undefined,{dateStyle:'medium', timeStyle:'short'}) : '', [slotIso]);

  return (
    <div className="max-w-[1100px] mx-auto p-4 grid gap-4 md:grid-cols-[320px_1fr]">
      <header className="md:col-span-2 flex items-center justify-between bg-white/5 rounded-2xl border border-white/10 px-4 py-3">
        <div className="flex items-center gap-3">
          <div className="w-9 h-9 rounded-xl" style={{background: 'conic-gradient(from 180deg, #8ef0cf, #50c8ff, #ff6b6b, #8ef0cf)'}}/>
          <h1 className="text-lg font-semibold">Vanilla · Rezervări</h1>
        </div>
        <div className="text-sm text-muted">UI React + TS · Vite · Tailwind</div>
      </header>

      <section className="card">
        <div className="card-h"><strong>Interval</strong></div>
        <div className="card-c space-y-3">
          <div className="flex gap-2 flex-wrap">
            <div className="grow">
              <label className="label">Data</label>
              <input className="input" type="date" value={date} onChange={e=>setDate(e.target.value)} />
            </div>
            <div>
              <label className="label">Ora</label>
              <input className="input w-[140px]" type="time" step={3600} value={time} onChange={e=>setTime(e.target.value)} />
            </div>
            <div>
              <label className="label">Durata</label>
              <select className="select w-[160px]" value={duration} onChange={e=>setDuration(Number(e.target.value))}>
                <option value={60}>60 min</option>
                <option value={90}>90 min</option>
                <option value={120}>120 min</option>
              </select>
            </div>
          </div>
          <div className="flex gap-2">
            <button className="btn btn-primary" onClick={refresh}>Verifică disponibilitatea</button>
          </div>
          <div className="flex gap-2 flex-wrap text-sm text-muted">
            <span className="pill"><span className="w-2.5 h-2.5 rounded-full" style={{background:'#8ef0cf'}}></span>Liber</span>
            <span className="pill"><span className="w-2.5 h-2.5 rounded-full" style={{background:'#ff6b6b'}}></span>Rezervat</span>
            <span className="text-xs">* Click pe o masă liberă pentru rezervare</span>
          </div>
        </div>
      </section>

      <section className="card">
        <div className="card-h"><strong>Hartă club</strong><small className="text-muted">&nbsp;– poziții exact ca în imagine</small></div>
        <div className="card-c">
          <Map mapSrc="/assets/vanilla-map.png" status={status} onClickFree={(id)=>setModal({ tableId:id, whenHuman, startAtIso: slotIso!, duration })} />
        </div>
      </section>

      <div className="toast" style={{display: toast? 'block':'none'}}>{toast}</div>

      <ReservationDialog
        open={!!modal}
        data={modal}
        onClose={()=>setModal(null)}
        onConfirm={handleCreate}
      />
    </div>
  );
}
