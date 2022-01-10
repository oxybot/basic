import { IconMoodConfuzed, IconMoodNervous } from "@tabler/icons";
import { useDispatch, useSelector } from "react-redux";
import { alertsState, hideAlert, removeAlert } from "./slice";

function Alert({ alert }) {
  const dispatch = useDispatch();
  const cssClass = alert.status;

  function handleClose() {
    dispatch(hideAlert(alert.identifier));
  }

  function handleTransitionEnd() {
    dispatch(removeAlert(alert.identifier));
  }

  return (
    <div
      id={`alert-${alert.identifier}`}
      className={`alert alert-${alert.category} fade ${cssClass}`}
      role="alert"
      onTransitionEnd={handleTransitionEnd}
    >
      <h3 className="alert-title">
        {alert.category === "error" && <IconMoodNervous />}
        {alert.category === "warning" && <IconMoodConfuzed />}
        <span className="ms-2">{alert.title}</span>
      </h3>
      <div>{alert.message}</div>
      <button className="btn-close position-absolute top-0 end-0 m-2" aria-label="close" onClick={handleClose}></button>
    </div>
  );
}

export default function AlertManager({ children }) {
  const alerts = useSelector(alertsState);

  return (
    <>
      {alerts.length > 0 && (
        <div className="alerts position-absolute top-0 end-0 p-3 w-100 w-lg-33" style={{ zIndex: 2000 }}>
          {alerts.map((alert) => (
            <Alert key={alert.identifier} alert={alert} />
          ))}
        </div>
      )}
      {children}
    </>
  );
}
