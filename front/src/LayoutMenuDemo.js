import { useState } from "react";
import { useDispatch } from "react-redux";
import { apiFetch } from "./api";
import { setRoles } from "./Authentication/slice";

export default function LayoutMenuDemo() {
  const dispatch = useDispatch();
  const [select, setSelect] = useState("all");
  const [beta, setBeta] = useState(true);

  function handleChange(event) {
    setSelect(event.target.value);

    update(event.target.value, beta);
  }

  function handleBeta(event) {
    setBeta(event.target.checked);
    update(select, event.target.checked);
  }

  function update(select, beta) {
    const betaArray = beta ? [{ code: "beta" }] : [];
    switch (select) {
      case "all":
        apiFetch("My/Roles", { method: "GET" }).then((response) => {
          if (!beta) {
            response = response.filter((v) => v.code !== "beta");
          }
          dispatch(setRoles(response));
        });
        break;

      case "user":
        dispatch(setRoles(betaArray.concat([])));
        break;

      case "hr":
        dispatch(setRoles(betaArray.concat([{ code: "time" }])));
        break;

      case "finance":
        dispatch(setRoles(betaArray.concat([{ code: "client" }])));
        break;

      default:
        console.error("Unknown type of user");
    }
  }

  return (
    <>
      <li className="nav-item">
        <select className="form-select" value={select} onChange={handleChange}>
          <option value="all">all</option>
          <option value="user">as user</option>
          <option value="hr">as HR</option>
          <option value="finance">as Finance</option>
        </select>
      </li>
      <div className="form-check form-switch mt-3">
        <input
          className="form-check-input"
          type="checkbox"
          role="switch"
          id="beta"
          checked={beta}
          onChange={handleBeta}
        />
        <label className="form-check-label" htmlFor="beta">
          Beta features
        </label>
      </div>
    </>
  );
}
