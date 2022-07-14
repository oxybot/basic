import dayjs from "dayjs";

export default function EntityFieldInputSchedule({ field, value, onChange }) {
  if (value === "") {
    value = [0, 0, 0, 0, 0, 0, 0];
  }
  const complex = Array.isArray(value) && value.length > 7;
  const prefix = field.name;
  const days = [0, 1, 2, 3, 4, 5, 6];

  function handleCheck(e) {
    const max = e.target.checked ? 14 : 7;
    if (value.length < max) {
      let updated = [...value];
      updated.push(0, 0, 0, 0, 0, 0, 0);
      console.log("option 1 : value = " + value + " ; update = " + updated);
      onChange({ target: { name: field.name, value: updated } });
    } else {
      let updated = value.slice(0, 7);
      console.log("option 2 : updated =" + updated);
      onChange({ target: { name: field.name, value: updated } });
    }
  }

  function handleChange(e, d) {
    let updated = [...value];
    updated[d] = Number(e.target.value);
    console.log("option 3 : updated =" + updated);
    onChange({ target: { name: field.name, value: updated } });
  }

  return (
    <>
      <div className="form-check form-switch">
        <input
          className="form-check-input"
          type="checkbox"
          role="switch"
          id={prefix + "-complex"}
          checked={complex}
          onChange={handleCheck}
        />
        <label className="form-check-label" htmlFor={prefix + "-complex"}>
          Odd/Even weeks mapping
        </label>
      </div>
      <div className="schedule">
        <div className="row g-0">
          {days.map((d) => {
            const dayLabel = dayjs().day(d).format("dddd").toLowerCase();
            const dayShort = dayjs().day(d).format("ddd");

            return (
              <div key={d} className="col">
                <label htmlFor={prefix + "-" + dayLabel} className="form-label text-center mb-1">
                  {dayShort}
                </label>
                <input
                  type="text"
                  className="form-control text-center"
                  id={prefix + "-" + dayLabel}
                  value={value[d] || 0}
                  onChange={(e) => handleChange(e, d)}
                />
              </div>
            );
          })}
        </div>
        {complex && (
          <div className="row mt-1 g-0">
            {days.map((d) => {
              const dayLabel = dayjs().day(d).format("dddd").toLowerCase();

              return (
                <div key={d} className="col">
                  <input
                    type="text"
                    className="form-control text-center"
                    id={prefix + "-" + dayLabel}
                    value={value[7 + d] || "0"}
                    onChange={(e) => handleChange(e, 7 + d)}
                  />
                </div>
              );
            })}
          </div>
        )}
      </div>
    </>
  );
}
