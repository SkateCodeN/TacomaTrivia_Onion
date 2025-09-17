export default function RowsPerPageSelect({ value, onChange, options = [10, 25, 50, 100] }) {
  return (
    <label style={{ display: 'inline-flex', gap: 8, alignItems: 'center' }}>
      Rows per page
      <select
        value={value}
        onChange={(e) => onChange(Number(e.target.value))}
        style={{ padding: '6px 10px', borderRadius: 8 }}
      >
        {options.map((n) => <option key={n} value={n}>{n}</option>)}
      </select>
    </label>
  );
}
