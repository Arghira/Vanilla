export type TablePos = { id: number; left: number; top: number };
// Approximate positions for 11 tables based on the provided map
const layout: TablePos[] = [
  { id: 1, left: 47, top: 23 },
  { id: 2, left: 51, top: 38 },
  { id: 3, left: 60, top: 26 },
  { id: 4, left: 64, top: 45 },
  { id: 5, left: 72, top: 46 },
  { id: 6, left: 46, top: 58 },
  { id: 7, left: 64, top: 60 },
  { id: 8, left: 71, top: 60 },
  { id: 9, left: 54, top: 76 },
  { id: 10, left: 61, top: 76 },
  { id: 11, left: 29, top: 78 }
];
export default layout;
