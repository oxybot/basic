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
        {alert.category === "danger" && <IconMoodNervous />}
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
  const fatals = alerts.filter((a) => a.category === "fatal");

  if (fatals.length > 0) {
    return (
      <div className="page page-center">
        <div className="container">
          <h1 className="text-center">Oups...</h1>

          <div className="row mt-5 g-5">
            <div className="col-12 col-lg-6">
              <div className="alert alert-important bg-blue w-60">Did it crash?</div>
              <div className="alert alert-important bg-green w-60 text-end ms-auto">Seems like it...</div>
              <div className="alert alert-important bg-indigo w-60">
                That's when the IT guys will ask for a refresh?
              </div>
              <div className="alert alert-important bg-green w-60 text-end ms-auto">
                Need to start somewhere
                <br />
                ...
              </div>
            </div>
            <div className="col-12 mt-5 mt-lg-0 col-lg-6 align-self-center">
              <h2>What can you do now?</h2>
              <p className="lead">First start to refresh the page, sometimes that really work &#128516;.</p>
              <p className="lead">If that is not enough, please raise the issue so that we can fix it.</p>
            </div>
          </div>
        </div>
      </div>
    );
  } else {
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
}
