import clsx from "clsx";

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

export default function EntityFieldInputColor({ field, value, hasErrors, onChange }) {
  return (
    <div className={clsx("row g-2", { "is-invalid": hasErrors })}>
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
