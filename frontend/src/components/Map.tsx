import layout from '../layout';
import type { TableStatus } from '../types';

export default function Map({
  mapSrc,
  status,
  onClickFree,
}: {
  mapSrc: string;
  status: TableStatus[];
  onClickFree: (tableId: number) => void;
}) {
  const statusMap = new Map(status.map(s => [s.tableId, s.isReserved]));

  return (
    <div className="stage">
      <img src={mapSrc} alt="Harta club" />
      <div className="absolute inset-0">
        {layout.map(pos => {
          const busy = statusMap.get(pos.id) === true;
          return (
            <button
              key={pos.id}
              className={`marker ${busy ? 'busy' : 'free'}`}
              style={{ left: `${pos.left}%`, top: `${pos.top}%` }}
              title={`Masa ${pos.id} · ${busy ? 'Rezervată' : 'Liberă'}`}
              onClick={() => !busy && onClickFree(pos.id)}
            >
              {pos.id}
            </button>
          );
        })}
      </div>
    </div>
  );
}
