import clsx from "clsx";
import { useState } from "react";

export default function Sections({ children }) {
  if (children === null) {
    children = [];
  } else if (!Array.isArray(children)) {
    children = [children];
  }

  let defaultCurrent = null;
  if (children.length > 0) {
    defaultCurrent = children[0].props.code;
  }

  const [current, setCurrent] = useState(defaultCurrent);

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
