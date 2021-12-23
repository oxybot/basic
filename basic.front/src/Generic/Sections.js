import clsx from "clsx";
import { useState } from "react";

export default function Sections({ children }) {
  const [current, setCurrent] = useState(
    children.length === 0 ? null : children[0].props.code
  );

  return (
    <>
      <ul className="nav mb-3 justify-content-center">
        {children &&
          children.map((section) => (
            <li key={section.props.code} className="nav-item">
              <button
                className={clsx("nav-link btn btn-nav", {
                  active: current === section.props.code,
                })}
                type="button"
                onClick={() => setCurrent(section.props.code)}
              >
                {section.props.children}
              </button>
            </li>
          ))}
      </ul>
      {children && children.filter((c) => c.props.code === current)}
    </>
  );
}
