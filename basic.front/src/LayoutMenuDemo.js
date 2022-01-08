import { useState } from "react";
import { useDispatch } from "react-redux";
import { apiFetch } from "./api";
import { setRoles } from "./Authentication/slice";

export default function LayoutMenuDemo() {
  const dispatch = useDispatch();
  const [select, setSelect] = useState("all");

  function handleChange(event) {
    setSelect(event.target.value);
    switch (event.target.value) {
      case "all":
        apiFetch("Roles/mine", { method: "GET" }).then((response) => dispatch(setRoles(response)));
        break;

      case "user":
        dispatch(setRoles([]));
        break;

      case "hr":
        dispatch(setRoles([{ code: "time" }]));
        break;

      case "finance":
        dispatch(setRoles([{ code: "client" }]));
        break;
    }
  }

  return (
    <li className="nav-item">
      <select className="form-select" value={select} onChange={handleChange}>
        <option value="all">all</option>
        <option value="user">as user</option>
        <option value="hr">as HR</option>
        <option value="finance">as Finance</option>
      </select>
    </li>
  );
}
