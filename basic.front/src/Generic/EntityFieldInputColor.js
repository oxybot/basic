const colors = [
  "blue",
  "azure",
  "indigo",
  "purple",
  "pink",
  "red",
  "orange",
  "yellow",
  "lime",
  "green",
  "teal",
  "cyan",
];

export default function EntityFieldInputColor({ field, value, onChange }) {
  return (
    <div className="row g-2">
      {colors.map((color) => (
        <div key={color} className="col-auto">
          <label className="form-colorinput">
            <input
              name={field.name}
              type="radio"
              value={"bg-" + color}
              className="form-colorinput-input"
              checked={"bg-" + color === value}
              onChange={onChange}
            />
            <span className={"form-colorinput-color bg-" + color} title={color}></span>
          </label>
        </div>
      ))}
    </div>
  );
}
